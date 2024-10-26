namespace Cinema.Domain.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId) : base($"The employee with id: {employeeId} doesn't exist in the database.")
        {
        }
    }
}
