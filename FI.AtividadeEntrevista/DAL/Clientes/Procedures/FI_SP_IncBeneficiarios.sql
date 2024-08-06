CREATE PROCEDURE FI_SP_IncBeneficiarios
    @IdCLIENTE BIGINT,
    @Beneficiarios dbo.BeneficiariosType READONLY  -- Tipo de tabela definido para receber a lista de beneficiários
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar se o IdCLIENTE é válido
    IF @IdCLIENTE IS NULL OR @IdCLIENTE <= 0
    BEGIN
        RAISERROR('ID do Cliente inválido.', 16, 1);
        RETURN;
    END

    -- Excluir todos os beneficiários existentes para o cliente
    DELETE FROM BENEFICIARIOS
    WHERE IDCLIENTE = @IdCLIENTE;

    -- Inserir os dados da tabela temporária na tabela final de beneficiários
    INSERT INTO BENEFICIARIOS (CPF, Nome, IdCLIENTE)
    SELECT CPF, Nome, @IdCLIENTE
    FROM @Beneficiarios;
END
