# Краткий обзор
База данных состоит из 2-ух таблиц Employees, Position
Код Employees:
```sql
CREATE TABLE "Employees" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"Name"	TEXT NOT NULL,
	"EnrollmentDate"	TEXT NOT NULL,
	"BaseSalary"	REAL NOT NULL,
	"ChiefId"	INTEGER,
	"PositionId"	INTEGER NOT NULL,
	"Login"	TEXT,
	"Password"	TEXT
)
```
Данные Employees:
```sql
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('1', 'A', '2019-05-18', '10000.0', '6', '1', 'A1');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('2', 'B', '2019-07-26', '10000.0', '5', '1', 'B2');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('3', 'C', '1990-09-19', '15000.0', '7', '1', 'C3');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('4', 'D', '2017-01-20', '12000.0', '9', '1', 'D4');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('5', 'E', '2017-01-20', '12000.0', '6', '2', 'D5');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('6', 'F', '2017-01-20', '18000.0', '7', '2', 'F6');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('7', 'G', '2015-05-15', '11500.0', '8', '2', 'G7');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('8', 'H', '2009-03-01', '12300.0', '10', '2', 'H8');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('9', 'I', '2001-02-12', '14000.0', '', '3', 'I9');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('10', 'J', '2013-05-20', '25000.0', '', '3', 'J10');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('11', 'K', '2019-07-26', '5000.0', '12', '3', 'K11');
INSERT INTO "main"."Employees" ("Id", "Name", "EnrollmentDate", "BaseSalary", "ChiefId", "PositionId", "Login") VALUES ('12', 'L', '2016-01-20', '2000.0', '', '3', 'L12');
```
Код Position:
```sql
CREATE TABLE "Position" (
	"Id"	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	"PositionName"	TEXT NOT NULL,
	"YearPercent"	REAL NOT NULL,
	"MaxYearPercent"	REAL NOT NULL
)
```
Данные Position:
```sql
INSERT INTO "main"."Position" ("Id", "PositionName", "YearPercent", "MaxYearPercent") VALUES ('1', 'Employee', '0.03', '0.3');
INSERT INTO "main"."Position" ("Id", "PositionName", "YearPercent", "MaxYearPercent") VALUES ('2', 'Manager', '0.05', '0.4');
INSERT INTO "main"."Position" ("Id", "PositionName", "YearPercent", "MaxYearPercent") VALUES ('3', 'Salesman', '0.01', '0.35');
```
Программа делалась на WPF (.NET Framework 4), с использованием паттерна MVVM, дополнительно взял библиотеку Fody.PropertyChanged. Структура классов в программе совпадает с БД (модели находятся в папке Models). В программе реализовано разграничение прав тестовый пользователи из каждого класса: Employee - логин:A1 пароль:A1, Manager - логин:F6 пароль:F6, Salesman - логин:J10 пароль:J10, Супер-пользователь - логин:admin пароль:admin. Всё, что относится к базовому заданию под пунктом "Требуется" расположено у супер-пользователя. В таблице "Сотрудники" выбираем сотрудника, снизу выбирается дата и по кнопке "Запрос зарплаты" выдается зарплата сотрудника на указанную дату. Кнопка "Запрос зарплаты всех" считает суммарную зарплату всех сотрудников фирмы в целом. На вкладке "Отчеты" находятся кнопки для отображения "Отчетов SQL" номера соответствуют таковым из задания (запросы находятся в папке Resources в текстовых файлах). Дополнительно у супер-пользователя есть функция добавления новых сотрудников разных видов на вкладке "Сотрудники" по кнопке "Добавить сотрудника".

Теперь по самим запросам. Я не очень хорош в SQL. Чтобы не разбираться вам во всём этом бардаке укажу этапы, по которым я их делал.

Сначала я сделал расчет кол-ва лет стажа от текущей даты:
```sql
SELECT
    (CASE 
        WHEN (SELECT CAST((strftime('%Y', date('now', '-1 month')) + strftime('%j', date('now', '-1 month')) / 365.2422) - (strftime('%Y', EnrollmentDate) + strftime('%j', EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = PositionId) >= (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = PositionId)
        THEN BaseSalary + BaseSalary * (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = PositionId)
        ELSE BaseSalary + BaseSalary * (SELECT CAST((strftime('%Y', date('now', '-1 month')) + strftime('%j', date('now', '-1 month')) / 365.2422) - (strftime('%Y', EnrollmentDate) + strftime('%j', EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = PositionId)
    END) as 'ЗП'
FROM Employees;
```
Потом составил запрос нахождения зарплаты для 1-го уровня иерархии подчиненных (Manager):
```sql
SELECT e.Name, e.BaseSalary + ifnull(SUM(s.BaseSalary),0)
FROM Employees e
LEFT JOIN Employees s on e.Id = s.ChiefId
GROUP BY e.Name;
```
Затем написал запрос для всей иерархии подчиненных (Salesman)
```sql
WITH Subordinates (Id, ParentId) AS (
	SELECT Id, Id as ParentId
	FROM Employees
	UNION ALL
	SELECT e.Id, s.ParentId
	FROM Employees e INNER JOIN Subordinates s ON (s.Id = e.ChiefId)
)
SELECT e.Name, sum(e1.BaseSalary)
FROM Subordinates s
JOIN Employees e on e.Id = s.ParentId
LEFT JOIN Employees e1 on e1.Id = s.Id and s.Id != s.ParentId
GROUP BY e.Name
ORDER BY e.Name;
```
Ну и наконец слепил это всё в один запрос. В данном случае это выбор всех сотрудников и их зарплат на текущую дату:
```sql
WITH Subordinates (Id, ParentId) AS (
	SELECT Id, Id as ParentId
	FROM Employees
	UNION ALL
	SELECT e.Id, s.ParentId
	FROM Employees e INNER JOIN Subordinates s ON (s.Id = e.ChiefId)
)

SELECT emain.Name,
            (CASE 
		    WHEN (SELECT CAST((strftime('%Y', date('now', '-1 month')) + strftime('%j', date('now', '-1 month')) / 365.2422) - (strftime('%Y', emain.EnrollmentDate) + strftime('%j', emain.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = emain.PositionId) >= (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = emain.PositionId)
            THEN emain.BaseSalary + emain.BaseSalary * (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = emain.PositionId)
		    ELSE emain.BaseSalary + emain.BaseSalary * (SELECT CAST((strftime('%Y', date('now', '-1 month')) + strftime('%j', date('now', '-1 month')) / 365.2422) - (strftime('%Y', emain.EnrollmentDate) + strftime('%j', emain.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = emain.PositionId)
	    END)
    + (
        Case
            WHEN emain.PositionId = 1 
                THEN 0
            WHEN emain.PositionId = 2 
                THEN (
                    SELECT ifnull(SUM(
                        CASE
                            WHEN (SELECT CAST((strftime('%Y', date('now')) + strftime('%j', date('now')) / 365.2422) - (strftime('%Y', s.EnrollmentDate) + strftime('%j', s.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = s.PositionId) >= (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = s.PositionId)
                            THEN s.BaseSalary + s.BaseSalary * (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = s.PositionId)
                            ELSE s.BaseSalary + s.BaseSalary * (SELECT CAST((strftime('%Y', date('now')) + strftime('%j', date('now')) / 365.2422) - (strftime('%Y', s.EnrollmentDate) + strftime('%j', s.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = s.PositionId)
                        END),0)
                    FROM Employees e
                    LEFT JOIN Employees s ON e.Id = s.ChiefId AND e.Id = emain.Id
                ) * 0.005
            WHEN emain.PositionId = 3
                THEN (
                    SELECT ifnull(SUM(
                        CASE
                            WHEN (SELECT CAST((strftime('%Y', date('now')) + strftime('%j', date('now')) / 365.2422) - (strftime('%Y', e1.EnrollmentDate) + strftime('%j', e1.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = e1.PositionId) >= (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = e1.PositionId)
                            THEN e1.BaseSalary + e1.BaseSalary * (SELECT p.MaxYearPercent FROM Position p WHERE p.Id = e1.PositionId)
                            ELSE e1.BaseSalary + e1.BaseSalary * (SELECT CAST((strftime('%Y', date('now')) + strftime('%j', date('now')) / 365.2422) - (strftime('%Y', e1.EnrollmentDate) + strftime('%j', e1.EnrollmentDate) / 365.2422) AS INT)) * (SELECT p.YearPercent FROM Position p WHERE p.Id = e1.PositionId)
                        END
                    ),0) as 'Pay'
                    FROM Subordinates s
                    JOIN Employees e ON e.Id = s.ParentId
                    LEFT JOIN Employees e1 ON e1.Id = s.Id AND s.Id != s.ParentId AND e.Id = emain.Id
                ) * 0.003
        END
    ) AS 'ЗП'
FROM Employees emain;
```
