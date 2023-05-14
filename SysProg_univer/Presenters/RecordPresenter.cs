using SysProgUniver.Views;
using System.Security.AccessControl;
using System;

namespace SysProgUniver.Presenters
{
    public class RecordPresenter
    {
        public string RUri
        {
            get => model.Url;
            set
            {
                model.Url = value;
                view.Url = value;
            }
        }
        public bool RAccessable
        {
            get => model.IsOpen;
            set
            {
                model.IsOpen = value;
                view.Accessable = value;
            }
        }

        public DateTime DT
        {
            get => model.CheckedAt;
            set
            {
                model.CheckedAt = value;
                view.DT = value;
            }
        }

        IRecord view;
        Record model;
        public RecordPresenter(Record record, IRecord view)
        {
            if (view == null) {
                throw new ArgumentNullException("view", "IRecord view not provided");
            }
            view.Url = record.Url;
            view.Accessable = record.IsOpen;
            view.DT = record.CheckedAt;
            this.view = view;
            this.model = record;
        }
    }
}
