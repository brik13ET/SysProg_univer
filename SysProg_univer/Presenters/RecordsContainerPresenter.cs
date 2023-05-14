using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysProgUniver.Views;

namespace SysProgUniver.Presenters
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
            Record[] FileRec;
            try
            {
                FileRec = Record.Extract(data);
            }
            catch (ArgumentException  ex)
            {
                view.Log = "Parsing file failed";
                view.Log = ex.Message;
                return;
            }catch (FormatException ex)
            {
                view.Log = "Parsing file failed";
                view.Log = ex.Message;
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
            using (var context = new DBContext(ConnectionString))
            {
                var promise = new Task(()=>
                {
                    foreach (Record rec in context.Records)
                        AddOrUpdate(rec);
                });

                Cursor.Current = Cursors.WaitCursor;
                promise.Start();
                while (!promise.IsCompleted && !promise.IsFaulted)
                {
                    Application.DoEvents();
                }
                promise.Dispose();
                Cursor.Current = Cursors.Default;
            }
            UIUpdate();
        }

        public void DbPush()
        {
            using (var context = new DBContext(ConnectionString))
            {
                context.Records.RemoveRange(from i in context.Records select i);
                context.Records.AddRange(container);
                context.SaveChanges();
            }
            UIUpdate();
        }

        private void UIUpdate()
        {
            var buf = new string[container.Count];
            for (int i = 0; i < container.Count; i++)
            {
                buf[i] = $"{container[i].CheckedAt}\t{container[i].IsOpen}\t{container[i].Url}\n";
            }
            view.RFancyOutput = buf;
        }
        public void Add(Record record)
        {
            var q = (from i in this.container.AsQueryable() where i.Url == record.Url select i);
            if (q.Count() != 0)
            {

                view.Log = "Has same element";
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
                view.Log = "No element";
                return;
            }

            find.ToArray()[0].IsOpen = record.IsOpen;
            find.ToArray()[0].CheckedAt = record.CheckedAt;
            UIUpdate();
        }
        public void Update(int index, Record record)
        {
            if (this.container.Count < index)
            {
                view.Log = "Index mismatch";
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
                view.Log = "No element. Using Add()";
                this.container.Add(record);
                UIUpdate();
                return;
            }
            view.Log = "Has element. Using Update()";
            find.ToArray()[0].IsOpen = record.IsOpen;
            find.ToArray()[0].CheckedAt = record.CheckedAt;

            UIUpdate();
        }

        public void Remove(Record record)
        {
            var find = (from i in this.container.AsQueryable() where i.Url == record.Url select i);
            if (find.Count() == 0)
            {
                view.Log = "No element";
                return;
            }
            this.container.Remove(record);
            UIUpdate();
        }
        public void Remove(int index)
        {
            if (this.container.Count < index)
            {
                view.Log = "Index mismatch";
                return;
            }
            this.container.RemoveAt(index);
            UIUpdate();
        }
    }
}
