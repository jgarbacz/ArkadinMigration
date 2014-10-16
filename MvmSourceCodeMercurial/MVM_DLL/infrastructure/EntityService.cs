using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVM
{
    public class MvmEntity
    {
        public string entityType;
        public string entityOperation;
    }

    // This class will be generated.
    public class MyBme : MvmEntity
    {
        // these all need to be decorated.
        public int idAcc;
        public DateTime startDate;
        public DateTime endDate;
        public MyBme()
        {
        }
    }

    // this class will be generated.
    //public class MyBme : MvmEntity
    //{
    //    // these all need to be decorated.
    //    public int idAcc;
    //    public DateTime startDate;
    //    public DateTime endDate;
    //    public MyBme()
    //    {
    //    }
    //}

    public class EntityService
    {
        public void Initialize()
        {
            //MVM mvm = new MVM();
            //mvm.Start(new string[] { "-server", "-parallel=1" });
            // need to load the appropriate config.
        }

        // need to generate a domain model

        public string CallEntity(ref MvmEntity mvmEntity)
        {
            // use reflection to copy fields off the entity onto an 
            // an mvm object.
            // based on c# type of the entity queue the proper procContext for it.

            // what about when you request entity menu, what do you get back?
            // you get back a domain model of a specific type that we've generated
            // that links BmeName->BmeClassName

            return "success";
        }

    }
}
