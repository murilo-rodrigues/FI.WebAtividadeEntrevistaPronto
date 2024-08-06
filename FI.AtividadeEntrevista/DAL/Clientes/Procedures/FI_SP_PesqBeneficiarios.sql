CREATE PROC FI_SP_PesqBeneficiarios
    @IdCliente BIGINT,
    @iniciarEm INT,
    @quantidade INT,
    @campoOrdenacao VARCHAR(200),
    @crescente BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SCRIPT NVARCHAR(MAX);
    DECLARE @CAMPOS NVARCHAR(MAX);
    DECLARE @ORDER VARCHAR(50);

    IF(@campoOrdenacao = 'CPF')
        SET @ORDER = ' CPF '
    ELSE
        SET @ORDER = ' NOME '

    IF(@crescente = 0)
        SET @ORDER = @ORDER + ' DESC'
    ELSE
        SET @ORDER = @ORDER + ' ASC'

    SET @CAMPOS = '@IdCliente BIGINT, @iniciarEm INT, @quantidade INT';
    SET @SCRIPT = 
    'SELECT ID, CPF, NOME FROM
        (SELECT ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row, ID, CPF, NOME FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IdCliente)
        AS BeneficiariosWithRowNumbers
    WHERE Row > @iniciarEm AND Row <= (@iniciarEm+@quantidade) ORDER BY ' + @ORDER;

    EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @IdCliente, @iniciarEm, @quantidade;

    -- Contar total de beneficiários
    SELECT COUNT(1) FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IdCliente;
END
