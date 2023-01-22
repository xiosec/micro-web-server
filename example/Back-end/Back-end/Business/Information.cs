using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Back_end.Data;
namespace Back_end.Business
{
    class Information : Iinformation
    {
        private PeoplesContext peoplesContext;
        public Information(PeoplesContext peoplesContext)
        {
            this.peoplesContext = peoplesContext;
        }
        public Data.Information Create(Data.Information information)
        {
            var value= peoplesContext.Information.Add(new Data.Information 
            {
                Firstname=information.Firstname,
                Lastname=information.Lastname,
                Email=information.Email,
                Address=information.Address,
                time=DateTime.Now
            }).Entity;
            peoplesContext.SaveChanges();
            return value;
        }

        public bool Delete(int Id)
        {
            var info = peoplesContext.Information.Find(Id);
            if (info!=null)
            {
                peoplesContext.Information.Remove(info);
                peoplesContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Data.Information> Find(string name)
        {
            return peoplesContext.Information.Where(info => info.Firstname == name).ToList();
        }

        public List<Data.Information> GetAllInformation()
        {
            return peoplesContext.Information.ToList();
        }

        public Data.Information Read(int Id)
        {
            return peoplesContext.Information.Where(info => info.Id == Id).Single();
        }

        public Data.Information Update(int Id, Data.Information information)
        {
            var info = peoplesContext.Information.Find(Id);
            if (info!=null)
            {
                info.Firstname = information.Firstname;
                info.Lastname = information.Lastname;
                info.Email = information.Email;
                info.Address = information.Address;
                peoplesContext.SaveChanges();
                information.Id = Id;
                information.time = DateTime.Now;
                return information;
            }
            return null;
        }
    }
}
