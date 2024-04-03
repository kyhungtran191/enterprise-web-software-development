namespace Server.Contracts.AcademicYears
{
    public class CreateAcademicYearRequest
    {
        public string AcademicYearName { get; set; } = null!;
        public DateTime StartClosureDate { get; set; }
        public DateTime EndClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }

    }
}
