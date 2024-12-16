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
            Summary = "������ �������",
            Description = "���� ����� ���������� �������� �� ������")]
        public IActionResult GetCreditInfo()
        {
            // ���������� ���������� � ��������� ������� API
            return Ok("API ��� ������� �������. ����������� POST ��� ������� � ���������� ������.");
        }

        // POST: api/CreditCalculator
        [HttpPost]
        [SwaggerOperation(
            Summary = "������ �������",
            Description = "���� ����� ����������� ������ ������� �� ��������� ����������")]
        public IActionResult CalculateCredit([FromBody] CreditRequest request)
        {
            if (request.LoanAmount <= 0 || request.InterestRate <= 0 || request.LoanTermMonths <= 0)
            {
                return BadRequest("��� �������� ������ ���� ������ ����.");
            }

            // ������������ ��������� �������
            var response = CalculateLoan(request);
            return Ok(response);
        }

        // POST: api/CreditCalculator/Application
        [HttpPost("Application")]
        [SwaggerOperation(
            Summary = "���������� ������ �� ������",
            Description = "���� ����� ����������� ������ ������� �� ��������� ���������� � ��������� ������")]
        public IActionResult SubmitApplication([FromBody] CreditApplicationRequest request)
        {
            if (request == null || request.LoanAmount <= 0 || request.InterestRate <= 0 || request.LoanTermMonths <= 0
                || string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest("������������ ������. ��������� ��������� ������� � ������ ������ ������������.");
            }

            // ������������ ��������� �������
            var response = CalculateLoan(request);

            // ��������� ������ � ������
            creditApplications.Add(request);

            // ���������� ����� � ������������� ������� � �������������� ���������� ������
            return Ok(new
            {
                Message = "������ �� ������ ������� ���������!",
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

        // ����� ��� ������� ���������� �������
        private CreditResponse CalculateLoan(CreditRequest request)
        {
            float monthlyRate = request.InterestRate / 100 / 12;
            int term = request.LoanTermMonths;

            // ������� ������������ �������
            var monthlyPayment = (float)((request.LoanAmount * monthlyRate) / (1 - Math.Pow(1 + monthlyRate, -term)));

            // ����� ������� � ��������
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
