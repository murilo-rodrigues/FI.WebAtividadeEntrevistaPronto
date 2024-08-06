CREATE PROCEDURE FI_SP_AltBeneficiario
    @Id BIGINT,
    @IdCLIENTE BIGINT,
    @CPF VARCHAR(14),
    @Nome VARCHAR(255)
AS
BEGIN
    -- Verificar se o Id é válido
    IF @Id IS NULL OR @Id <= 0
    BEGIN
        RAISERROR('ID do Beneficiário inválido.', 16, 1);
        RETURN;
    END

    -- Verificar se o IdCLIENTE é válido
    IF @IdCLIENTE IS NULL OR @IdCLIENTE <= 0
    BEGIN
        RAISERROR('ID do Cliente inválido.', 16, 1);
        RETURN;
    END

    -- Atualizar o beneficiário na tabela BENEFICIARIOS
    UPDATE BENEFICIARIOS
    SET CPF = @CPF,
        Nome = @Nome,
        IdCLIENTE = @IdCLIENTE
    WHERE ID = @Id;
END
