namespace Server.Application.Common.Dtos.AcademicYears
{
    public class AcademicYearDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartClosureDate { get; set; }
        public DateTime EndClosureDate { get; set; }
        public DateTime FinalClosureDate { get; set; }
        public required string UserNameCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime DateCreated { get; set; } = default!;
        public DateTime? DateDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
