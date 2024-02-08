USE [master]
GO
CREATE DATABASE [BankingApp]
GO
USE [BankingApp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Tablas
CREATE TABLE [dbo].[CreditCards](
	[CreditCardID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[Cardholder] [nvarchar](255) NOT NULL,
	[CardNumber] [nvarchar](16) NOT NULL,
	[CreditLimit] [decimal](18, 2) NOT NULL,
	[CurrentBalance] [decimal](18, 2) NOT NULL,
	[AvailableBalance] [decimal](18, 2) NOT NULL,
	[BonifiableInterestRate] [decimal](5, 2) NOT NULL,
	[MinimumBalanceInterestRate] [decimal](5, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CreditCardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [uniqueidentifier] NOT NULL,
	[CreditCardID] [uniqueidentifier] NULL,
	[Date] [date] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchases]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchases](
	[PurchaseID] [uniqueidentifier] NOT NULL,
	[CreditCardID] [uniqueidentifier] NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Date] [date] NOT NULL,
 CONSTRAINT [PK__Purchase__6B0A6BDEC3A0EB65] PRIMARY KEY CLUSTERED 
(
	[PurchaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__User__1788CCACBDDC46BB] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CreditCards]  WITH CHECK ADD  CONSTRAINT [FK__CreditCar__UserI__4BAC3F29] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[CreditCards] CHECK CONSTRAINT [FK__CreditCar__UserI__4BAC3F29]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([CreditCardID])
REFERENCES [dbo].[CreditCards] ([CreditCardID])
GO
ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK__Purchase__Credit__4E88ABD4] FOREIGN KEY([CreditCardID])
REFERENCES [dbo].[CreditCards] ([CreditCardID])
GO
ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK__Purchase__Credit__4E88ABD4]
GO

--StoredProcedures

/****** Object:  StoredProcedure [dbo].[spAccountStatus]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAccountStatus]
--EXEC spAccountStatus @CreditCardID = '9AD1FCAA-5303-47D0-83F5-A108B1B03F7A'
    @CreditCardID uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @PorcentajeInteresBonificable decimal(5, 2);
	DECLARE @PorcentajeConfigurableSaldoMínimo decimal(5, 2);
    SELECT @PorcentajeInteresBonificable = BonifiableInterestRate, 
	@PorcentajeConfigurableSaldoMínimo = MinimumBalanceInterestRate
    FROM dbo.CreditCards
    WHERE CreditCardID = @CreditCardID;

    DECLARE @EstadoCuentaTabla TABLE (
		IdTarjeta uniqueidentifier,
        TitularTarjeta nvarchar(255),
        NumeroTarjeta nvarchar(16),
        SaldoActual decimal(18, 2),
        LimiteCredito decimal(18, 2),
        SaldoDisponible decimal(18, 2),
        TotalComprasMesActual decimal(18, 2),
        TotalComprasMesAnterior decimal(18, 2),
        InteresBonificable decimal(18, 2),
        CuotaMinimaAPagar decimal(18, 2),
        MontoTotalContadoIntereses decimal(18, 2),
        MontoTotalSinIntereses decimal(18, 2)
    );

    INSERT INTO @EstadoCuentaTabla (IdTarjeta,TitularTarjeta, NumeroTarjeta, SaldoActual, LimiteCredito, SaldoDisponible)
    SELECT cc.CreditCardID, cc.Cardholder, cc.CardNumber, cc.CurrentBalance, cc.CreditLimit, cc.AvailableBalance
    FROM dbo.CreditCards cc
    WHERE cc.CreditCardID = @CreditCardID;

    UPDATE @EstadoCuentaTabla
    SET TotalComprasMesActual = ISNULL((SELECT SUM(p.Amount)
                                        FROM dbo.Purchases p
                                        WHERE p.CreditCardID = @CreditCardID
                                          AND MONTH(p.Date) = MONTH(GETDATE())), 0),
        TotalComprasMesAnterior = ISNULL((SELECT SUM(p.Amount)
                                          FROM dbo.Purchases p
                                          WHERE p.CreditCardID = @CreditCardID
                                            AND MONTH(p.Date) = MONTH(DATEADD(MONTH, -1, GETDATE()))), 0);

    UPDATE @EstadoCuentaTabla
    SET InteresBonificable = SaldoActual * @PorcentajeInteresBonificable,
        CuotaMinimaAPagar = SaldoActual * @PorcentajeConfigurableSaldoMínimo,
        MontoTotalContadoIntereses = SaldoActual + (SaldoActual * @PorcentajeInteresBonificable),
        MontoTotalSinIntereses = SaldoActual;

    SELECT * FROM @EstadoCuentaTabla;
END
GO
/****** Object:  StoredProcedure [dbo].[spAuthUser]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spAuthUser]
@Email nvarchar(255),
@Password nvarchar(255)
AS
BEGIN
SELECT UserId, Username, Email
FROM Users
WHERE Email = @Email AND Password = @Password
END
GO
/****** Object:  StoredProcedure [dbo].[spCreatePayment]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCreatePayment]
    @PaymentId uniqueidentifier,
    @CreditCardId uniqueidentifier,
    @Date DATE,
    @Amount DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO Payments (PaymentId, CreditCardId, Date, Amount)
    VALUES (@PaymentId, @CreditCardId, @Date, @Amount);
END
GO
/****** Object:  StoredProcedure [dbo].[spCreatePurchase]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCreatePurchase]
    @PurchaseId uniqueidentifier,
    @CreditCardId uniqueidentifier,
    @Description NVARCHAR(255),
    @Amount DECIMAL(18, 2),
    @Date DATE
AS
BEGIN
    INSERT INTO Purchases (PurchaseId, CreditCardId, Description, Amount, Date)
    VALUES (@PurchaseId, @CreditCardId, @Description, @Amount, @Date);
END;
GO
/****** Object:  StoredProcedure [dbo].[spCreateUser]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spCreateUser]
@UserId uniqueidentifier,
@Username varchar(255),
@Email varchar(255),
@Password varchar(255),
@FirstName varchar(255),
@LastName varchar(255)
AS
BEGIN
INSERT INTO Users (UserId, Username, Email, Password, FirstName, LastName) VALUES (@UserId, @Username, @Email, @Password, @FirstName, @LastName)
END
GO
/****** Object:  StoredProcedure [dbo].[spGetCreditCardTransactions]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spGetCreditCardTransactions]
--EXEC spGetCreditCardTransactions @CreditCardId = '9AD1FCAA-5303-47D0-83F5-A108B1B03F7A'
@CreditCardId uniqueidentifier
AS
SELECT 
    'Compra' AS TransactionType,
    Date AS TransactionDate,
    Amount AS TransactionAmount
FROM [dbo].Purchases
WHERE [CreditCardID] = @CreditCardId  -- Reemplaza con el ID de tu tarjeta de crédito
    AND MONTH(Date) = MONTH(GETDATE())  -- Filtra por compras del mes actual

UNION

-- Consulta para obtener pagos
SELECT 
    'Pago' AS TransactionType,
    Date AS TransactionDate,
    Amount AS TransactionAmount
FROM [dbo].Payments
WHERE [CreditCardID] = @CreditCardId  -- Reemplaza con el ID de tu tarjeta de crédito
    AND MONTH(Date) = MONTH(GETDATE())  -- Filtra por pagos del mes actual

ORDER BY TransactionDate DESC;  -- Ordena por fecha de forma descendente
GO
/****** Object:  StoredProcedure [dbo].[spGetCreditCardTransactionsByUserId]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetCreditCardTransactionsByUserId]
-- EXEC spGetCreditCardTransactionsByUserId @UserId = 'AFD774A8-1953-407B-802E-03477181F78B'
    @UserId uniqueidentifier
AS
BEGIN
    SELECT 
        'Compra' AS TransactionType,
        Date AS TransactionDate,
        Amount AS TransactionAmount
    FROM [dbo].Purchases p
    INNER JOIN [dbo].CreditCards cc ON p.CreditCardID = cc.CreditCardID
    WHERE cc.UserId = @UserId
        AND MONTH(p.Date) = MONTH(GETDATE())  -- Filtra por compras del mes actual

    UNION

    SELECT 
        'Pago' AS TransactionType,
        Date AS TransactionDate,
        Amount AS TransactionAmount
    FROM [dbo].Payments pm
    INNER JOIN [dbo].CreditCards cc ON pm.CreditCardID = cc.CreditCardID
    WHERE cc.UserId = @UserId
        AND MONTH(pm.Date) = MONTH(GETDATE())  -- Filtra por pagos del mes actual

    ORDER BY TransactionDate DESC;  -- Ordena por fecha de forma descendente
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPaymentsByCreditCard]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPaymentsByCreditCard]
    @CreditCardId uniqueidentifier
AS
BEGIN
    SELECT *
    FROM Payments
    WHERE CreditCardId = @CreditCardId;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPaymentsByUserId]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPaymentsByUserId]
--EXEC spGetPaymentsByUserId @UserId = 'AFD774A8-1953-407B-802E-03477181F78B'
    @UserId uniqueidentifier
AS
BEGIN
    SELECT p.*
    FROM Payments p
    INNER JOIN CreditCards cc ON p.CreditCardId = cc.CreditCardId
    WHERE cc.UserId = @UserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetPurchasesByCreditCard]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPurchasesByCreditCard]
    @CreditCardId uniqueidentifier
AS
BEGIN
    SELECT *
    FROM Purchases
    WHERE CreditCardId = @CreditCardId;
END
GO
/****** Object:  StoredProcedure [dbo].[spGetPurchasesByUserId]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetPurchasesByUserId]
    @UserId uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Purchases
    WHERE CreditCardId IN (SELECT CreditCardId FROM CreditCards WHERE UserId = @UserId);
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetUserByEmail]    Script Date: 8/2/2024 16:40:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetUserByEmail]
@Email nvarchar(255)
AS
BEGIN
SELECT * FROM Users
WHERE Email = @Email
END
GO
----------Poblar Info
INSERT INTO Users VALUES('59E184F2-53F9-469D-953B-9AAB3434B402','user','contrasena2024','correo@correo.com','Juan','López')
INSERT INTO Users VALUES('56fef1a7-8846-46c3-b939-9ecc096e4e10','user2','contrasena2024','correo2@correo.com','Lorena','López')
INSERT INTO CreditCards VALUES('0DB61AED-3644-41B5-A291-E99512E2A43B','59E184F2-53F9-469D-953B-9AAB3434B402','Juan López','4111111111111111',1000,0,1000,0.25,0.05)
INSERT INTO CreditCards VALUES('EFF33F47-9965-40D9-931B-D1FFE93F0DB0','59E184F2-53F9-469D-953B-9AAB3434B402','Juan López','5111111111111111',1000,0,1000,0.25,0.10)
INSERT INTO CreditCards VALUES('d1da4412-0cbf-4031-a410-e9262a641c23','56fef1a7-8846-46c3-b939-9ecc096e4e10','Lorena López','5111111111111111',1000,0,1000,0.25,0.10)

---------Triggers
USE [BankingApp]
GO

/****** Object:  Trigger [dbo].[UpdateCreditCardBalances]    Script Date: 8/2/2024 16:42:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[UpdateCreditCardBalances]
ON [dbo].[Payments]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CreditCardID uniqueidentifier;
    DECLARE @Amount decimal(18, 2);

    -- Obtenemos el ID de la tarjeta y el monto del pago insertado
    SELECT @CreditCardID = inserted.CreditCardID,
           @Amount = inserted.Amount
    FROM inserted;

    -- Actualizamos el campo CurrentBalance de la tarjeta
    UPDATE CreditCards
    SET CurrentBalance = CurrentBalance - @Amount
    WHERE CreditCardID = @CreditCardID;

    -- Actualizamos el campo AvailableBalance de la tarjeta
    UPDATE CreditCards
    SET AvailableBalance = AvailableBalance + @Amount
    WHERE CreditCardID = @CreditCardID;
END;
GO

ALTER TABLE [dbo].[Payments] ENABLE TRIGGER [UpdateCreditCardBalances]
GO

/****** Object:  Trigger [dbo].[UpdateBalanceTrigger]    Script Date: 8/2/2024 16:55:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[UpdateBalanceTrigger]
ON [dbo].[Purchases]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar CurrentBalance en CreditCards
    UPDATE cc
    SET cc.CurrentBalance = cc.CurrentBalance + i.Amount
    FROM CreditCards cc
    INNER JOIN inserted i ON cc.CreditCardID = i.CreditCardID;

    -- Actualizar AvailableBalance en CreditCards
    UPDATE cc
    SET cc.AvailableBalance = cc.AvailableBalance - i.Amount
    FROM CreditCards cc
    INNER JOIN inserted i ON cc.CreditCardID = i.CreditCardID;
END;
GO

ALTER TABLE [dbo].[Purchases] ENABLE TRIGGER [UpdateBalanceTrigger]
GO