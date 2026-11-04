namespace Data_PrintFarm_Manager.Entities;

// Explicit many-to-many junction table, as in HospitalManager's DoctorPatient entity.
public class PrintJobFilament
{
    public int PrintJobId { get; set; }
    public PrintJob PrintJob { get; set; } = null!;

    public int FilamentId { get; set; }
    public Filament Filament { get; set; } = null!;
}
