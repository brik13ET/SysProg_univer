using SysProg_univer.Views;
using System.Security.AccessControl;
using System;

namespace SysProg_univer.Presenters
{
    public class RecordPresenter
    {
        public string RUri { get => model.Url; set => model.Url = value; }
        public bool RAccessable { get => model.isOpen; set => model.isOpen = value; }

        IRecord view;
        Record model;
        public RecordPresenter(Record record, IRecord view)
        {
            view.URL = record.Url;
            view.Accessable = record.isOpen;
            this.view = view;
            this.model = record;
        }
    }
}
