using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MetraTech.Custom.Services.Mvm.MvmActivityService.ClientProxies;
namespace MvmActivityServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            MvmActivityService_CallMvmEntity_Client client= new MvmActivityService_CallMvmEntity_Client();

            // create an entity and set some fields
            var entity = new MetraTech.DomainModel.MvmTypes.automated_conferencing_per_minute_rates();
          
            entity.Start_date = DateTime.Parse("1/1/2008");
            entity.End_date = DateTime.Parse("1/1/2010");
            entity.Id_acc = 1234451;
            entity.Operation = "get";

            // setup the client
            List<MetraTech.DomainModel.MvmTypes.MvmBaseObject> mvmEntities = new List<MetraTech.DomainModel.MvmTypes.MvmBaseObject>();
            mvmEntities.Add(entity);
            client.InOut_mvmEntities = mvmEntities;
            client.UserName = "su";
            client.Password = "su123";

            // call the client
            Console.WriteLine("start invoking mvm:"+DateTime.Now);
            client.Invoke();
            Console.WriteLine("done invoking mvm:" + DateTime.Now);

            mvmEntities = (List<MetraTech.DomainModel.MvmTypes.MvmBaseObject>)client.InOut_mvmEntities;

            Console.WriteLine("Logging the returned entity properties:");
            foreach (var outEntity in mvmEntities)
            {
                foreach (var p in outEntity.GetProperties())
                {
                    var name = p.Name;
                    var val = outEntity.GetValue(p);
                    Console.WriteLine(name + "=[" + (val != null ? val.ToString() : "null") + "], type=" + p.PropertyType);
                }
            }


            Console.WriteLine("hit key to exit");
            Console.ReadKey();
        }
    }
}
