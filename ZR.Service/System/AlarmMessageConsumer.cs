using Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZR.Common;
using ZR.Model.System;
using ZR.Service.System.IService;
using ZR.ServiceCore.Signalr;

namespace ZR.Service.System
{
    public class AlarmMessageConsumer : BackgroundService
    {
        private readonly ILogger<AlarmMessageConsumer> _logger;
        private readonly ISysAlarmRecordService _alarmRecordService;
        private readonly ISysTaskService _taskService;
        private readonly ISysCameraService _cameraService;
        private readonly ISysAlgorithmService _algorithmService;
        private readonly IHubContext<MessageHub> _hubContext;
        private IConnection _connection;
        private IChannel _channel;
        private readonly string _queueName;

        // 缓存键前缀
        private const string CACHE_KEY_TASK = "task:";
        private const string CACHE_KEY_CAMERA = "camera:";
        private const string CACHE_KEY_ALGORITHM = "algorithm:";
        // 缓存过期时间（分钟）
        private const int CACHE_EXPIRE_MINUTES = 30;

        public AlarmMessageConsumer(
            ILogger<AlarmMessageConsumer> logger,
            ISysAlarmRecordService alarmRecordService,
            ISysTaskService taskService,
            ISysCameraService cameraService,
            ISysAlgorithmService algorithmService,
            IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _alarmRecordService = alarmRecordService;
            _taskService = taskService;
            _cameraService = cameraService;
            _algorithmService = algorithmService;
            _hubContext = hubContext;
            _queueName = AppSettings.GetConfig("RabbitMQ:QueueName");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = AppSettings.GetConfig("RabbitMQ:HostName"),
                UserName = AppSettings.GetConfig("RabbitMQ:UserName"),
                Password = AppSettings.GetConfig("RabbitMQ:Password")
            };

            try
            {
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                await _channel.QueueDeclareAsync(_queueName, durable: false, exclusive: false, autoDelete: false);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = JsonConvert.DeserializeObject<AlarmMessage>(Encoding.UTF8.GetString(body));

                        // 从缓存或数据库获取相关信息
                        var task = GetTaskFromCache(message.TaskId);
                        var camera = GetCameraFromCache(message.CameraCode);
                        var algorithm = GetAlgorithmFromCache(message.AlgorithmCode);

                        if (task == null || camera == null || algorithm == null)
                        {
                            _logger.LogError($"Task, camera or algorithm not found. TaskId: {message.TaskId}, CameraCode: {message.CameraCode}, AlgorithmCode: {message.AlgorithmCode}");
                            await _channel.BasicAckAsync(ea.DeliveryTag, false);
                            return;
                        }

                        // 保存图片
                        var imageBytes = Convert.FromBase64String(message.Image);
                        var timestamp = DateTimeOffset.FromUnixTimeSeconds(message.Timestamp);
                                          
                        // 转换为本地时间
                        timestamp = timestamp.ToLocalTime();

                        var imagePath = SaveImage(imageBytes, timestamp);

                        // 创建报警记录
                        var alarmRecord = new SysAlarmRecord
                        {
                            TaskName = task.TaskName,
                            AlgorithmCode = algorithm.AlgorithmCode,
                            AlgorithmName = algorithm.AlgorithmName,
                            AlarmImage = imagePath,
                            CameraName = camera.CameraName,
                            CameraCode = camera.CameraCode,
                            StreamUrl = message.RtspUrl,
                            AlarmTime = timestamp.DateTime,
                            AlarmResult = JsonConvert.SerializeObject(message.Detection)
                        };

                        // 保存报警记录
                        var alarmId = await Task.Run(() => _alarmRecordService.Insertable(alarmRecord).ExecuteReturnSnowflakeId(), stoppingToken);
                        alarmRecord.AlarmId = alarmId;

                        // 构造报警通知信息
                        var alarmNotification = new
                        {
                            alarmId = alarmRecord.AlarmId,
                            taskName = alarmRecord.TaskName,
                            algorithmName = alarmRecord.AlgorithmName,
                            cameraName = alarmRecord.CameraName,
                            alarmTime = alarmRecord.AlarmTime,
                            alarmImage = alarmRecord.AlarmImage, 
                            detection = message.Detection,
                            streamUrl = alarmRecord.StreamUrl
                        };

                        // 通过SignalR发送报警信息给所有在线用户
                        await _hubContext.Clients.All.SendAsync("receiveAlarm", alarmNotification);

                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing alarm message");
                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    }
                };

                await _channel.BasicConsumeAsync(_queueName, false, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error establishing RabbitMQ connection or channel.");
                throw;
            }

            await Task.CompletedTask;
        }

        private SysTask GetTaskFromCache(string taskId)
        {
            var cacheKey = $"{CACHE_KEY_TASK}{taskId}";
            var task = CacheHelper.GetCache<SysTask>(cacheKey);

            if (task == null)
            {
                task = _taskService.Queryable().First(t => t.TaskId.ToString() == taskId);
                if (task != null)
                {
                    CacheHelper.SetCache(cacheKey, task, CACHE_EXPIRE_MINUTES);
                }
            }

            return task;
        }

        private SysCamera GetCameraFromCache(string cameraCode)
        {
            var cacheKey = $"{CACHE_KEY_CAMERA}{cameraCode}";
            var camera = CacheHelper.GetCache<SysCamera>(cacheKey);

            if (camera == null)
            {
                camera = _cameraService.Queryable().First(c => c.CameraCode == cameraCode);
                if (camera != null)
                {
                    CacheHelper.SetCache(cacheKey, camera, CACHE_EXPIRE_MINUTES);
                }
            }

            return camera;
        }

        private SysAlgorithm GetAlgorithmFromCache(string algorithmCode)
        {
            var cacheKey = $"{CACHE_KEY_ALGORITHM}{algorithmCode}";
            var algorithm = CacheHelper.GetCache<SysAlgorithm>(cacheKey);

            if (algorithm == null)
            {
                algorithm = _algorithmService.Queryable().First(a => a.AlgorithmCode == algorithmCode);
                if (algorithm != null)
                {
                    CacheHelper.SetCache(cacheKey, algorithm, CACHE_EXPIRE_MINUTES);
                }
            }

            return algorithm;
        }

        private string SaveImage(byte[] imageBytes, DateTimeOffset timestamp)
        {
            var baseDir = AppSettings.GetConfig("AlarmImage:SavePath");
            var relativePath = Path.Combine(
                timestamp.Year.ToString(),
                timestamp.Month.ToString("00"),
                timestamp.Day.ToString("00"),
                timestamp.Hour.ToString("00")
            );
            var fullDir = Path.Combine(baseDir, relativePath);
            Directory.CreateDirectory(fullDir);

            var fileName = $"{timestamp.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid():N}.jpg";
            var fullPath = Path.Combine(fullDir, fileName);
            File.WriteAllBytes(fullPath, imageBytes);

            // 返回带前缀的路径
            return $"/alarm/image/{fileName}";
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
