using ServiceStack.Web;

namespace ServiceStack.Northwind.ServiceInterface
{
    using System;
    using System.IO;
    using ServiceModel.Operations;
    using ServiceModel.Types;

    public class VCardFormat
    {
        private const string VCardContentType = "text/x-vcard";

        public static void Register(IAppHost appHost)
        {
            appHost.ContentTypes.Register(VCardContentType, SerializeToStream, DeserializeFromStream);

            appHost.GlobalResponseFilters.Add((req, res, dto) =>
            {
                if (req.ResponseContentType == VCardContentType)
                {
                    res.AddHeader(HttpHeaders.ContentDisposition,
                        string.Format("attachment;filename={0}.vcf", req.OperationName));
                }
            });
        }

        public static void SerializeToStream(IRequest req, object response, Stream stream)
        {
            var customerResponse = response as CustomerDetailsResponse;
            using (var sw = new StreamWriter(stream))
            {
                if (customerResponse != null)
                {
                    WriteCustomer(sw, customerResponse.Customer);
                }
                var customers = response as CustomersResponse;
                if (customers != null)
                {
                    customers.Customers.ForEach(x => WriteCustomer(sw, x));
                }
            }
        }

        public static void WriteCustomer(StreamWriter sw, Customer customer)
        {
            sw.WriteLine("BEGIN:VCARD");
            sw.WriteLine("VERSION:2.1");
            sw.WriteLine("FN:" + customer.ContactName);
            sw.WriteLine("ORG:" + customer.CompanyName);
            sw.WriteLine("TITLE:" + customer.ContactTitle);
            sw.WriteLine("EMAIL;TYPE=PREF,INTERNET:" + customer.Email);
            sw.WriteLine("TEL;HOME;VOICE:" + customer.Phone);
            sw.WriteLine("TEL;WORK;FAX:" + customer.Fax);
            sw.WriteLine("ADR;TYPE=HOME;"
                         + new[] { customer.Address, customer.City, customer.PostalCode }.Join(";"));
            sw.WriteLine("END:VCARD");
        }

        public static object DeserializeFromStream(Type type, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}