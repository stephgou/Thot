using Doc.Search.App.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Search.App.Models
{
    public partial class DocumentProperties
    {
        public DocumentProperties(Action<DocumentProperties> onUpload) : this()
        {
            this.onUpload = onUpload;
        }

        private Action<DocumentProperties> onUpload;
        private RelayCommand uploadCommand;
        public RelayCommand UploadCommand
        {
            get
            {
                if (uploadCommand == null)
                    uploadCommand = new RelayCommand(
                           () =>
                           {
                               if (onUpload != null)
                                   onUpload(this);
                           },
                        null
                    );
                return uploadCommand;
            }
        }
    }
}
