CREATE PROC FI_SP_ConsBeneficiarios
    @IdCLIENTE BIGINT
AS
BEGIN
    IF(ISNULL(@IdCLIENTE, 0) = 0)
        SELECT CPF, Nome, ID, IdCLIENTE
        FROM Beneficiarios WITH(NOLOCK)
    ELSE
        SELECT CPF, Nome, ID, IdCLIENTE
        FROM Beneficiarios WITH(NOLOCK)
        WHERE IdCLIENTE = @IdCLIENTE
END
