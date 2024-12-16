using Microsoft.AspNetCore.Mvc;
using credit_calculation;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace CreditCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCalculatorController : ControllerBase
    {
        private static List<CreditApplicationRequest> creditApplications = new List<CreditApplicationRequest>();

        // GET: api/CreditCalculator
        [HttpGet]
        [SwaggerOperation(
            Summary = "Доступ сервиса",
            Description = "Этот метод определяет доступен ли сервис")]
        public IActionResult GetCreditInfo()
        {
            // Возвращает информацию о доступных методах API
            return Ok("API для расчета кредита. Используйте POST для расчета и оформления заявки.");
        }

        // POST: api/CreditCalculator
        [HttpPost]
        [SwaggerOperation(
            Summary = "Расчет кредита",
            Description = "Этот метод расчитывает ставку кредита по введенным параметрам")]
        public IActionResult CalculateCredit([FromBody] CreditRequest request)
        {
            if (request.LoanAmount <= 0 || request.InterestRate <= 0 || request.LoanTermMonths <= 0)
            {
                return BadRequest("Все значения должны быть больше нуля.");
            }

            // Рассчитываем параметры кредита
            var response = CalculateLoan(request);
            return Ok(response);
        }

        // POST: api/CreditCalculator/Application
        [HttpPost("Application")]
        [SwaggerOperation(
            Summary = "Оформление заявки на кредит",
            Description = "Этот метод расчитывает ставку кредита по введенным параметрам и оформляет заявку")]
        public IActionResult SubmitApplication([FromBody] CreditApplicationRequest request)
        {
            if (request == null || request.LoanAmount <= 0 || request.InterestRate <= 0 || request.LoanTermMonths <= 0
                || string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("Некорректные данные. Проверьте параметры кредита и личные данные пользователя.");
            }

            // Рассчитываем параметры кредита
            var response = CalculateLoan(request);

            // Сохраняем заявку в памяти
            creditApplications.Add(request);

            // Возвращаем ответ с рассчитанными данными и подтверждением сохранения заявки
            return Ok(new
            {
                Message = "Заявка на кредит успешно сохранена!",
                CreditDetails = response,
                UserInfo = new
                {
                    request.FirstName,
                    request.LastName,
                    request.MiddleName,
                    request.PhoneNumber
                }
            });
        }

        // Метод для расчета параметров кредита
        private CreditResponse CalculateLoan(CreditRequest request)
        {
            float monthlyRate = request.InterestRate / 100 / 12;
            int term = request.LoanTermMonths;

            // Формула аннуитетного платежа
            var monthlyPayment = (float)((request.LoanAmount * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -term)));

            // Общая выплата и проценты
            var totalPayment = monthlyPayment * term;
            var totalInterest = totalPayment - request.LoanAmount;

            return new CreditResponse
            {
                MonthlyPayment = monthlyPayment,
                TotalPayment = totalPayment,
                TotalInterest = totalInterest
            };
        }
    }
}
