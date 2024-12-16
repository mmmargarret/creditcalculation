namespace credit_calculation
{
    public class CreditResponse
    {
        public float MonthlyPayment { get; set; }     // ����������� ������
        public float TotalPayment { get; set; }       // ����� ����� �������
        public float TotalInterest { get; set; }      // ����� ����� ���������
    }
    // ������� ������ ��� ������� �������
    public class CreditRequest
    {
        public float LoanAmount { get; set; }         // ����� �������
        public float InterestRate { get; set; }       // ���������� ������
        public int LoanTermMonths { get; set; }       // ���� ������� (� �������)
    }

    // ����������� ������ ��� ���������� ������ � ������� ������������
    public class CreditApplicationRequest : CreditRequest
    {
        public string FirstName { get; set; }         // ���
        public string LastName { get; set; }          // �������
        public string MiddleName { get; set; }        // ��������
        public string PhoneNumber { get; set; }       // ����� ��������
    }
}
