using ExpenseManager.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Models
{
    public class ExpensesDataAcessLayer : IExpenseService
    {
        private ExpenseDbContext db;

        public ExpensesDataAcessLayer(ExpenseDbContext _db)
        {
            db = _db;
        }
        public void AddExpense(ExpenseReport expense)
        {
            try
            {
                db.ExpenseReports.Add(expense);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

       

        public void DeleteExpense(int id)
        {
            try
            {
                ExpenseReport emp = db.ExpenseReports.Find(id);
                db.ExpenseReports.Remove(emp);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<ExpenseReport> GetAllExpenses()
        {
            try
            {
                return db.ExpenseReports.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ExpenseReport GetExpenseData(int id)
        {
            try
            {
                ExpenseReport expense = db.ExpenseReports.Find(id);
                return expense;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<ExpenseReport> GetSearchResult(string searchString)
        {
            List<ExpenseReport> exp = new List<ExpenseReport> ();
            try
            {
                exp = GetAllExpenses().ToList();
                return exp.Where(x=>x.ItemName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase)!=-1);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateExpense(ExpenseReport expense)
        {
            try
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Dictionary<string, decimal> CalculateMonthlyExpense()
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictMonthlySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReports.Where
                (cat => cat.Category == "Food" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.ExpenseReports.Where
               (cat => cat.Category == "Shopping" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.ExpenseReports.Where
               (cat => cat.Category == "Travel" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.ExpenseReports.Where
               (cat => cat.Category == "Health" && (cat.ExpenseDate > DateTime.Now.AddMonths(-7)))
               .Select(cat => cat.Amount)
               .Sum();

            dictMonthlySum.Add("Food", foodSum);
            dictMonthlySum.Add("Shopping", shoppingSum);
            dictMonthlySum.Add("Travel", travelSum);
            dictMonthlySum.Add("Health", healthSum);

            return dictMonthlySum;
        }

        public Dictionary<string, decimal> CalculateWeeklyExpense()
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();

            Dictionary<string, decimal> dictWeeklySum = new Dictionary<string, decimal>();

            decimal foodSum = db.ExpenseReports.Where
                (cat => cat.Category == "Food" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
                .Select(cat => cat.Amount)
                .Sum();

            decimal shoppingSum = db.ExpenseReports.Where
               (cat => cat.Category == "Shopping" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal travelSum = db.ExpenseReports.Where
               (cat => cat.Category == "Travel" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            decimal healthSum = db.ExpenseReports.Where
               (cat => cat.Category == "Health" && (cat.ExpenseDate > DateTime.Now.AddDays(-28)))
               .Select(cat => cat.Amount)
               .Sum();

            dictWeeklySum.Add("Food", foodSum);
            dictWeeklySum.Add("Shopping", shoppingSum);
            dictWeeklySum.Add("Travel", travelSum);
            dictWeeklySum.Add("Health", healthSum);

            return dictWeeklySum;
        }
    }
}
