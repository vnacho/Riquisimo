Comandos para realizar migraciones desde Package Manager Console (PMC) :

Añadir migración => EntityFrameworkCore\Add-Migration estadoPedido -Context ApplicationDbContext
Actualizar base de datos => EntityFrameworkCore\Update-database -Context ApplicationDbContext
Script => EntityFrameworkCore\Script-Migration -From <PreviousMigration> -To <LastMigration> -Context ApplicationDbContext

Borrar última migration => EntityFrameworkCore\Remove-migration -Context ApplicationDbContext
