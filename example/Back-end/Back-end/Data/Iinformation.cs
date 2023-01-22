using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Back_end.Data
{
    interface Iinformation
    {
        Information Create(Information information);
        Information Read(int Id);
        Information Update(int Id, Information information);
        bool Delete(int Id);
        List<Information> GetAllInformation();
        List<Information> Find(string name);
    }
}
