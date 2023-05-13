using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SysProg_univer.Views;

namespace SysProg_univer.Presenters
{
    public class RecordsContainerPresenter
    {
        private List<Record> container = new List<Record>();

        private string ConnectionString;
        private IRecordsContainer view;

        public Record this[int index]
        {
            get => container[index];
        }

        public int Length { get => container.Count; }

        public RecordsContainerPresenter(string ConnectionString, IRecordsContainer view)
        {
            this.ConnectionString = ConnectionString;
            this.view = view;
        }
        public void Load(string filename)
        {
            string data = File.ReadAllText(filename);
            Record[] FileRec = null;
            try
            {
                FileRec = Record.Extract(data);
            }
            catch (ArgumentException ex)
            {
                view.RLog = "Parsing file failed";
                view.RLog = ex.Message;
                return;
            }
            foreach (Record rec in FileRec)
            {
                AddOrUpdate(rec);
            }
            UIUpdate();
        }

        public void Save(string filename)
        {
            string data = Record.Compress(container.ToArray());
            File.WriteAllText(filename, data);
            UIUpdate();
        }
        public void DbPull()
        {
            using (var context = new DbContext(ConnectionString))
            {
                foreach (Record rec in context.records)
                {
                    AddOrUpdate(rec);
                }
            }
            UIUpdate();
        }

        public void DbPush()
        {
            using (var context = new DbContext(ConnectionString))
            {
                context.records.RemoveRange(from i in context.records select i);
                context.records.AddRange(container);
                context.SaveChanges();
            }
            UIUpdate();
        }

        private void UIUpdate()
        {
            var buf = new string[container.Count];
            for (int i = 0; i < container.Count; i++)
            {
                buf[i] = $"{container[i].isOpen}\t{container[i].Url}\n";
            }
            view.RFancyOutput = buf;
        }
        public void Add(Record record)
        {
            var q = (from i in this.container.AsQueryable() where i.Url == record.Url select i);
            if (q.Count() != 0)
            {

                view.RLog = "Has same element";
                return;
            }
            this.container.Add(record);
            UIUpdate();
        }

        public void Update(Record record)
        {
            var find = (from i in this.container.AsQueryable() where i.Url == record.Url select i); 
            if (find.Count() == 0)
            {
                view.RLog = "No element";
                return;
            }

            find.ToArray()[0].isOpen = record.isOpen;
            UIUpdate();
        }
        public void Update(int index, Record record)
        {
            if (this.container.Count < index)
            {
                view.RLog = "Index mismatch";
                return;
            }
            this.container[index] = record;
            UIUpdate();
        }
        public void AddOrUpdate(Record record)
        {
            var find = (from i in this.container.AsQueryable() where i.Url == record.Url select i);
            if (find.Count() == 0)
            {
                view.RLog = "No element. Using Add()";
                this.container.Add(record);
                UIUpdate();
                return;
            }
            view.RLog = "Has element. Using Update()";
            find.ToArray()[0].isOpen = record.isOpen;

            UIUpdate();
        }

        public void Remove(Record record)
        {
            var find = (from i in this.container.AsQueryable() where i.Url == record.Url select i);
            if (find.Count() == 0)
            {
                view.RLog = "No element";
                return;
            }
            this.container.Remove(record);
            UIUpdate();
        }
        public void Remove(int index)
        {
            if (this.container.Count < index)
            {
                view.RLog = "Index mismatch";
                return;
            }
            this.container.RemoveAt(index);
            UIUpdate();
        }
    }
}
