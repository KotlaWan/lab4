using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using laba2.Models;

namespace laba2.Services
{
    public class DbService
    {
        private ApplicationContext _db;

        public DbService(ApplicationContext context)
        {
            _db = context;
        }

        public List<PriceClass> GetPriceFuels()
        {
            var priceFuels = from ClassType in _db.ClassTypes
                             join Class in _db.Classes
                             on ClassType.Id equals Class.ClassTypeId
                             select new { ClassType = ClassType.Name, Cla = Class.ClassLead, Class = Class.Count, date = Class.Date };
            List<PriceClass> list = new List<PriceClass>();
            foreach (var pf in priceFuels) list.Add(new PriceClass(pf.ClassType, pf.Cla , pf.Class, pf.date ));

            return list;
        }

        public IQueryable<PriceClass> GetPriceFuels(string typeFule, SortState sortOrder = SortState.ClassLeadAsc)
        {
            IQueryable<PriceClass> priceFuels = from ClassType in _db.ClassTypes
                                                join Class in _db.Classes
                                                on ClassType.Id equals Class.ClassTypeId
                                                select new PriceClass(ClassType.Name, Class.ClassLead, Class.Count, Class.Date);
            if (!String.IsNullOrEmpty(typeFule))
            {
                priceFuels = priceFuels.Where(p => p.ClassType.Contains(typeFule));
            }

            switch (sortOrder)
            {
                case SortState.ClassLeadDesc:
                    priceFuels = priceFuels.OrderByDescending(s => s.ClassLead);
                    break;
                case SortState.ClassTypeAsc:
                    priceFuels = priceFuels.OrderBy(s => s.ClassType);
                    break;
                case SortState.ClassTypeDesc:
                    priceFuels = priceFuels.OrderByDescending(s => s.ClassType);
                    break;
                case SortState.DateAsc:
                    priceFuels = priceFuels.OrderBy(s => s.Date);
                    break;
                case SortState.DateDesc:
                    priceFuels = priceFuels.OrderByDescending(s => s.Date);
                    break;
                default:
                    priceFuels = priceFuels.OrderBy(s => s.ClassLead);
                    break;
            }

            //List<PriceFuels> list = new List<PriceFuels>();
            //foreach (var pf in priceFuels) list.Add(new PriceFuels(pf.type, pf.price, pf.date));

            return priceFuels;
        }

        public void AddPriceFuels(PriceClass priceFuels)
        {
            //ClassType classType = new ClassType { Name = Char.ConvertFromUtf32(Char.ConvertToUtf32("о", 0)) , Description = "Стандартный" };
            //_db.ClassTypes.Add(classType);
            //_db.SaveChanges();

            Class newClass = new Class { ClassLead = priceFuels.ClassLead, Count = priceFuels.Count, Date = DateTime.Now, ClassTypeId = priceFuels.ClassTypeId };
            _db.Classes.Add(newClass);
            _db.SaveChanges();
        }

        public IQueryable<Subject> GetSubjects(SortState sortOrder = SortState.NameAsc)
        {
            var volume = _db.Subjects.Select(v => v);

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    volume = volume.OrderByDescending(s => s.Name);
                    break;
                case SortState.TeacherAsc:
                    volume = volume.OrderBy(s => s.Teacher);
                    break;
                case SortState.TeacherDesc:
                    volume = volume.OrderByDescending(s => s.Teacher);
                    break;
                default:
                    volume = volume.OrderBy(s => s.Name);
                    break;
            }

            return volume;
        }
    }
}
