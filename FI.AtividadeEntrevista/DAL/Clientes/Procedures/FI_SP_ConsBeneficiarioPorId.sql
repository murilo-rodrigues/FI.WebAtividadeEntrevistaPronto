CREATE PROCEDURE FI_SP_ConsBeneficiarioPorId
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        IdCLIENTE,
        CPF,
        Nome
    FROM 
        Beneficiarios
    WHERE 
        Id = @Id;

    SET NOCOUNT OFF;
END;
