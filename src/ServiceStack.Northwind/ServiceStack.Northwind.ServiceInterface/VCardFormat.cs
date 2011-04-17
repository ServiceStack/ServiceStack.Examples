using System;
using System.IO;
using ServiceStack.Common.Web;
using ServiceStack.Northwind.ServiceModel.Operations;
using ServiceStack.Northwind.ServiceModel.Types;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace ServiceStack.Northwind.ServiceInterface
{
	public class VCardFormat
	{
		private const string ContentType = "text/directory";

		public static void Register(IAppHost appHost)
		{
			appHost.ContentTypeFilters.Register(ContentType, SerializeToStream, DeserializeFromStream);

			appHost.ResponseFilters.Add((req, res, dto) =>
			{
				if (req.ResponseContentType == ContentType)
				{
					res.AddHeader(HttpHeaders.ContentDisposition,
						string.Format("attachment;filename={0}.vcf", req.OperationName));
				}
			});
		}

		public static void SerializeToStream(IRequestContext requestContext, object response, Stream stream)
		{
			var customerDetailsResponse = response as CustomerDetailsResponse;
			using (var sw = new StreamWriter(stream))
			{
				if (customerDetailsResponse != null)
				{
					WriteCustomer(sw, customerDetailsResponse.Customer);
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