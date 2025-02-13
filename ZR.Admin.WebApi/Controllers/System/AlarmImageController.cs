using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using IO = System.IO; // 为 System.IO 定义别名 IO
using System.Text.RegularExpressions;
using ZR.Admin.WebApi.Filters;

namespace ZR.Admin.WebApi.Controllers
{
    /// <summary>
    /// 报警图片控制器
    /// </summary>
    [Route("/alarm/image")]
    public class AlarmImageController : BaseController
    {
        /// <summary>
        /// 获取报警图片
        /// </summary>
        /// <param name="filename">图片文件名，格式：yyyyMMddHHmmss_guid.jpg</param>
        /// <returns></returns>
        [HttpGet("{filename}")]
        [AllowAnonymous]
        public IActionResult GetImage(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    return NotFound();
                }

                // 使用正则表达式从文件名中提取日期时间部分
                var match = Regex.Match(filename, @"^(\d{4})(\d{2})(\d{2})(\d{2})");
                if (!match.Success)
                {
                    return BadRequest("Invalid filename format");
                }

                // 提取年月日时
                var year = match.Groups[1].Value;
                var month = match.Groups[2].Value;
                var day = match.Groups[3].Value;
                var hour = match.Groups[4].Value;

                // 构造完整的图片路径
                var baseDir = AppSettings.GetConfig("AlarmImage:SavePath");
                var relativePath = Path.Combine(year, month, day, hour);
                var imagePath = Path.Combine(baseDir, relativePath, filename);

                // 检查文件是否存在
                if (!IO.File.Exists(imagePath))
                {
                    return NotFound();
                }

                // 读取图片文件
                var imageBytes = IO.File.ReadAllBytes(imagePath);

                // 根据文件扩展名设置Content-Type
                var contentType = "image/jpeg"; // 默认为JPEG
                var extension = Path.GetExtension(filename).ToLower();
                switch (extension)
                {
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                }

                return new FileContentResult(imageBytes, contentType);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}