namespace credit_calculation
{
    public class CreditResponse
    {
        public float MonthlyPayment { get; set; }     // Ежемесячный платеж
        public float TotalPayment { get; set; }       // Общая сумма выплаты
        public float TotalInterest { get; set; }      // Общая сумма процентов
    }
    // Базовый запрос для расчета кредита
    public class CreditRequest
    {
        public float LoanAmount { get; set; }         // Сумма кредита
        public float InterestRate { get; set; }       // Процентная ставка
        public int LoanTermMonths { get; set; }       // Срок кредита (в месяцах)
    }

    // Расширенный запрос для оформления заявки с данными пользователя
    public class CreditApplicationRequest : CreditRequest
    {
        public string FirstName { get; set; }         // Имя
        public string LastName { get; set; }          // Фамилия
        public string MiddleName { get; set; }        // Отчество
        public string PhoneNumber { get; set; }       // Номер телефона
    }
}
