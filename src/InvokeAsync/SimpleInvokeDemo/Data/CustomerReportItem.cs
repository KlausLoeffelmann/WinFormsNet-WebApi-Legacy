using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleInvokeDemo.Data
{
    public class CustomerReportItem
    {
        private static Contact[] s_contacts=Contact.GetDemoData(20).ToArray();
        private static Random s_random = new Random(DateTime.Now.Millisecond);

        public int IDCustomer { get; set; }
        public string CustomerName { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public Decimal RevenueInDollar { get; set; }

        public static CustomerReportItem GetCustomerReportItem(int IDCustomer)
        {
            System.Threading.Thread.SpinWait(1000000 * (5 + s_random.Next(50)));
            var contact = s_contacts[s_random.Next(20)];
            return new CustomerReportItem()
            {
                IDCustomer = IDCustomer,
                CustomerName = $"{contact.LastName}, {contact.FirstName}",
                City = contact.City,
                Zip = contact.Zip,
                RevenueInDollar = (decimal) s_random.NextDouble() * 1_000_000
            };
        }

        public async static Task<CustomerReportItem> GetCustomerReportItemAsync(int IDCustomer)
        {
            return await Task.Run(() => GetCustomerReportItem(IDCustomer));
        }

        public static void AddCustomerReportToBindingList(BindingList<CustomerReportItem> bindingList, int IDCustomer)
        {
            var customer = GetCustomerReportItem(IDCustomer);
            bindingList.Add(customer);
        }

        /// <summary>
        /// Aggregates the Results for the Customer and adds it to the BindingList. IMPORTANT: Needs to run on UI-Thread!
        /// </summary>
        /// <param name="bindingList"></param>
        /// <param name="IDCustomer"></param>
        /// <returns></returns>
        public async static Task AddCustomerReportToBindingListAsync(BindingList<CustomerReportItem> bindingList, int IDCustomer)
        {
            var customer = await GetCustomerReportItemAsync(IDCustomer);
            bindingList.Add(customer);
        }
    }
}
