using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Lombiq.DownloadAs.Models;
using Orchard.ContentManagement;
using Orchard.FileSystems.Media;
using Orchard.Services;
using Orchard.Settings;
using Piedone.HelpfulLibraries.Utilities;

namespace Lombiq.DownloadAs.Services
{
    public class FileBuilder : IFileBuilder
    {
        private readonly ISiteService _siteService;
        private readonly IContainerFlattener _flattener;
        private readonly IEnumerable<IFileBuilderWorker> _fileBuilderWorkers;
        private readonly IStorageProvider _storageProvider;
        private readonly IClock _clock;

        private const string CacheFolderPath = "_LombiqModules/DownloadAs/CacheFiles/";


        public FileBuilder(
            ISiteService siteService,
            IContainerFlattener flattener,
            IEnumerable<IFileBuilderWorker> fileBuilderWorkers,
            IStorageProvider storageProvider,
            IClock clock)
        {
            _siteService = siteService;
            _flattener = flattener;
            _fileBuilderWorkers = fileBuilderWorkers;
            _storageProvider = storageProvider;
            _clock = clock;
        }


        public IEnumerable<IFileBuildWorkerDescriptor> GetWorkers()
        {
            return _fileBuilderWorkers.Select(worker => worker.Descriptor);
        }

        public bool HasWorkerFor(string extension)
        {
            return _fileBuilderWorkers.Any(w => string.Equals(w.Descriptor.SupportedFileExtension, extension, StringComparison.OrdinalIgnoreCase));
        }

        public IFileResult Build(IContent content, string extension)
        {
            ThrowIfInvalidArguments(content, extension);
            return Build(content, extension, false);
        }

        public IFileResult BuildRecursive(IContent content, string extension)
        {
            ThrowIfInvalidArguments(content, extension);
            return Build(content, extension, true);
        }


        private IFileResult Build(IContent content, string extension, bool recursive)
        {
            var filePath = CacheFolderPath + content.ContentItem.Id + "-" + recursive + "." + extension;
            var mimeType = MimeAssistant.GetMimeType(filePath);

            if (_storageProvider.FileExists(filePath))
            {
                var file = _storageProvider.GetFile(filePath);
                if (file.GetLastUpdated().ToUniversalTime().Add(_siteService.GetSiteSettings().As<DownloadAsSettingsPart>().CacheTimeout) >= _clock.UtcNow)
                {
                    return new FileResult(() => file.OpenRead(), mimeType);
                }
                else
                {
                    _storageProvider.DeleteFile(filePath);
                }
            }

            IEnumerable<IContent> contents;
            if (recursive) contents = _flattener.Flatten(content);
            else contents = new[] { content };

            var worker = _fileBuilderWorkers.Where(w => string.Equals(w.Descriptor.SupportedFileExtension, extension, StringComparison.OrdinalIgnoreCase)).LastOrDefault();

            if (worker == null) throw new NotSupportedException("There is no worker for building a file of type " + extension + ".");

            var stream = worker.Build(contents);

            var newFile = _storageProvider.CreateFile(filePath);
            using (var writeStream = newFile.OpenWrite())
            {
                stream.CopyTo(writeStream);
            }

            return new FileResult(() =>
            {
                stream.Position = 0;
                return stream;
            }, mimeType);
        }


        private static void ThrowIfInvalidArguments(IContent content, string extension)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (string.IsNullOrEmpty(extension)) throw new ArgumentNullException("extensions");
        }


        private class FileResult : IFileResult
        {
            private Func<Stream> _streamFactory;

            public string MimeType { get; private set; }


            public FileResult(Func<Stream> streamFactory, string mimeType)
            {
                _streamFactory = streamFactory;
                MimeType = mimeType;
            }


            public Stream OpenRead()
            {
                return _streamFactory();
            }
        }
    }
}