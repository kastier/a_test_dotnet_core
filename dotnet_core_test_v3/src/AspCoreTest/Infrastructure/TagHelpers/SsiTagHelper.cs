using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AspCoreTest.Infrastructure.TagHelpers
{
    [HtmlTargetElement("ssi-include", Attributes ="file")]
    public class SsiTagHelper:TagHelper
    {
        private const string CachePrefix = "SsiInclude:";

        private const int ReadBufferSize = 1024 * 10;

        private ILogger<SsiTagHelper> _logger;

        private IMemoryCache _memoryCache;

        private IHostingEnvironment _hostingEnvironment;

        public string File { get; set; }

        public SsiTagHelper(ILogger<SsiTagHelper> logger, IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }

        //TODO 优化，考虑使用ProcessAsync和异步IO
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = LoadFileContentWithCache(File);
            output.TagName = string.Empty;
            output.Content.SetHtmlContent(content);
        }
        
        private string LoadFileContentWithCache(string file)
        {

            var cacheKey = CachePrefix + file;
            string fileContent;
            if (!_memoryCache.TryGetValue(cacheKey, out fileContent))
            {
                try
                {
                    /* TODO 以前用的是string.Intern(Path.GetFileName(file))做同步，这里还需要完善
                   var fileName = string.Intern(Path.GetFileName(file)); 
                    //按文件名做同步
                   lock (fileName)
                    {
                        fileContent = HttpRuntime.Cache.Get(cacheKey) as string;
                        if (fileContent == null)
                        {
                    */
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    var readFromFileList = new List<string>();
                    fileContent = LoadFileContentRecursively(file, readFromFileList);

                    /*
                    //依赖文件变化的缓存
                    HttpRuntime.Cache.Insert(cacheKey, fileContent, new CacheDependency(readFromFileList.ToArray()));
                    */
                   var cacheEntry = _memoryCache.CreateEntry(cacheKey);
                    _memoryCache.Set(cacheKey, fileContent, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                    stopWatch.Stop();
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("Successful loaded {0} from disk ,taken {1} ms.", file, stopWatch.ElapsedMilliseconds);
                    }
                    /*     
                     }
                 }
                     */

                }
                catch (Exception ex)
                {
                    _logger.LogError("Error happens when loading {0},details:{1}",File, ex);
                }
            }

            return fileContent;
        }

        private string LoadFileContentRecursively(string file, List<string> readFromFileList)
        {
            var filePhysicalPath = _hostingEnvironment.ContentRootPath + file;
            readFromFileList.Add(filePhysicalPath);
            var fileContent = ReadFile(filePhysicalPath);

            var regex = new Regex("<!--\\s*#include\\s+file=\"(.*)\"\\s*-->");
            var matches = regex.Matches(fileContent);
            foreach (Match match in matches)
            {
                var wholeIncludeTag = match.Groups[0].ToString();
                var includeFileAttribute = match.Groups[1].ToString();
                var includeFileContent = LoadFileContentRecursively(includeFileAttribute, readFromFileList);
                fileContent = fileContent.Replace(wholeIncludeTag, includeFileContent);
            }

            return fileContent;
        }

        public string ReadFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                using(var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, ReadBufferSize))
                using (var sr = new StreamReader(fs,System.Text.Encoding.GetEncoding("utf-8")))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                _logger.LogError("Error happens,{0} not exists.", filePath);
                return string.Empty;
            }
        }
    }
}
