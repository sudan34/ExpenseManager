using ExpenseManager.Interface;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ExpenseManager.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expense;

        public ExpenseController(IExpenseService expense)
        {
            _expense = expense;
        }
        public IActionResult Index(string searchString)
        {
            List<ExpenseReport> lstEmployee = new List<ExpenseReport>();
            lstEmployee = _expense.GetAllExpenses().ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                lstEmployee = _expense.GetSearchResult(searchString).ToList();
            }
            return View(lstEmployee);
        }
        
        public ActionResult AddEditExpenses(int itemId)
        {
            ExpenseReport model = new ExpenseReport();
            if (itemId > 0)
            {
                model = _expense.GetExpenseData(itemId);
            }
            return PartialView("_expenseForm", model);
        }

        [HttpPost]
        public ActionResult Create(ExpenseReport newExpense)
        {
            if (ModelState.IsValid)
            {
                if (newExpense.ItemId > 0)
                {
                    _expense.UpdateExpense(newExpense);
                }
                else
                {
                    _expense.AddExpense(newExpense);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _expense.DeleteExpense(id);
            return RedirectToAction("Index");
        }

        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }

        public JsonResult GetMonthlyExpense()
        {
            Dictionary<string, decimal> monthlyExpense = _expense.CalculateMonthlyExpense();
            return new JsonResult(monthlyExpense);
        }

        public JsonResult GetWeeklyExpense()
        {
            Dictionary<string, decimal> weeklyExpense = _expense.CalculateWeeklyExpense();
            return new JsonResult(weeklyExpense);
        }
    }
}
