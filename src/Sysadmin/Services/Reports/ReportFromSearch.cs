using FastReport;
using FastReport.ReportBuilder;
using LdapForNet;
using Sysadmin.Models;
using SysAdmin.ActiveDirectory.Services.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysadmin.Services.Reports
{
    public class ReportFromSearch : IReport
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Group { get; private set; }

        private readonly string filter;
        private readonly Dictionary<string, string> columns;

        public ReportFromSearch(string group, string name, string description, string filter, Dictionary<string, string> columns)
        {
            Name = name;
            Description = description;
            Group = group;

            this.filter = filter;
            this.columns = columns;
        }

        public async Task<Report> Report()
        {
            List<LdapEntry> list = new List<LdapEntry>();

            await Task.Run(async () =>
            {
                using (var ldap = new LdapService(App.SERVER, App.CREDENTIAL))
                {
                    list = await ldap.SearchAsync(filter);
                }
            });

            list = list.OrderBy(c => c.Dn).ToList();

            if (columns.Count == 1)
                return OneColumnsReport(list);

            if (columns.Count == 2)
                return TwoColumnsReport(list);

            if (columns.Count == 3)
                return ThreeColumnsReport(list);

            throw new ArgumentException(nameof(columns));
        }

        private Report OneColumnsReport(List<LdapEntry> list)
        {

            List<ReportItem> items = new List<ReportItem>();

            KeyValuePair<string, string> column1 = columns.ToList()[0];

            foreach (LdapEntry entry in list)
            {
                ReportItem reportItem = new ReportItem();

                var item1 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column1.Key.ToLower());

                if (item1.Value != null)
                    reportItem.ColumnOne = string.Join(", ", item1.Value);

                items.Add(reportItem);
            }

            var builder = new ReportBuilder();

            var report = builder.Report(items)
            .ReportTitle(title => title
                .Text(Name + " / " + Description)
                .HorzAlign(HorzAlign.Center)
             )
            .DataHeader(header => header
                .Font("Helvetica")
             )
            .Data(data =>
            {
                data.Column(col => col.ColumnOne).Title(column1.Value);
            })
            .Prepare();

            return report;
        }

        private Report TwoColumnsReport(List<LdapEntry> list)
        {

            List<ReportItem> items = new List<ReportItem>();

            KeyValuePair<string, string> column1 = columns.ToList()[0];
            KeyValuePair<string, string> column2 = columns.ToList()[1];

            foreach (LdapEntry entry in list)
            {
                ReportItem reportItem = new ReportItem();

                var item1 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column1.Key.ToLower());
                var item2 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column2.Key.ToLower());

                if (item1.Value != null)
                    reportItem.ColumnOne = string.Join(", ", item1.Value);

                if (item2.Value != null)
                    reportItem.ColumnTwo = string.Join(", ", item2.Value);

                items.Add(reportItem);
            }

            var builder = new ReportBuilder();

            var report = builder.Report(items)
            .ReportTitle(title => title
                .Text(Name + " / " + Description)
                .HorzAlign(HorzAlign.Center)
             )
            .DataHeader(header => header
                .Font("Helvetica")
             )
            .Data(data =>
            {
                data.Column(col => col.ColumnOne).Title(column1.Value);
                data.Column(col => col.ColumnTwo).Title(column2.Value);
            })
            .Prepare();

            return report;
        }

        private Report ThreeColumnsReport(List<LdapEntry> list)
        {

            List<ReportItem> items = new List<ReportItem>();

            KeyValuePair<string, string> column1 = columns.ToList()[0];
            KeyValuePair<string, string> column2 = columns.ToList()[1];
            KeyValuePair<string, string> column3 = columns.ToList()[2];

            foreach (LdapEntry entry in list)
            {
                ReportItem reportItem = new ReportItem();

                var item1 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column1.Key.ToLower());
                var item2 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column2.Key.ToLower());
                var item3 = entry.Attributes.FirstOrDefault(c => c.Key.ToLower() == column3.Key.ToLower());

                if (item1.Value != null)
                    reportItem.ColumnOne = string.Join(", ", item1.Value);

                if (item2.Value != null)
                    reportItem.ColumnTwo = string.Join(", ", item2.Value);

                if (item3.Value != null)
                {
                    reportItem.ColumnThree = string.Join(", ", item3.Value);

                    if (reportItem.ColumnThree.EndsWith(".0Z"))
                    {
                        int year = Convert.ToInt32(reportItem.ColumnThree.Substring(0, 4));
                        int month = Convert.ToInt32(reportItem.ColumnThree.Substring(4, 2));
                        int day = Convert.ToInt32(reportItem.ColumnThree.Substring(6, 2));

                        int hour = 0;
                        int minute = 0;
                        int second = 0;

                        if (reportItem.ColumnThree.Length > 8)
                        {
                            hour = Convert.ToInt32(reportItem.ColumnThree.Substring(8, 2));
                            minute = Convert.ToInt32(reportItem.ColumnThree.Substring(10, 2));
                            second = Convert.ToInt32(reportItem.ColumnThree.Substring(12, 2));
                        }
                        reportItem.ColumnThree = new DateTime(year, month, day, hour, minute, second).ToString();
                    }

                    if (reportItem.ColumnThree.Length == 18)
                        try
                        {
                            reportItem.ColumnThree = DateTime.FromFileTime(Int64.Parse(reportItem.ColumnThree)).ToString();
                        }
                        catch
                        {
                            reportItem.ColumnThree = string.Empty;
                        }

                }

                items.Add(reportItem);
            }

            var builder = new ReportBuilder();

            var report = builder.Report(items)
            .ReportTitle(title => title
                .Text(Name + " / " + Description)
                .HorzAlign(HorzAlign.Center)
             )
            .DataHeader(header => header
                .Font("Helvetica")
             )
            .Data(data =>
            {
                data.Column(col => col.ColumnOne).Title(column1.Value);
                data.Column(col => col.ColumnTwo).Title(column2.Value);
                data.Column(col => col.ColumnThree).Title(column3.Value);
            })
            .Prepare();

            return report;
        }

    }

}