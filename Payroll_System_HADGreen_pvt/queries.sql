select empId as 'ID', empName as 'Name', hRate as 'Hourly Rate' from employee
where empName like '%Rahal Amrith%'


delete from employee where empId = 10

update employee set empName = 'Methma Amandi', hRate = 300 where empId = 8


select * from employee where empName = 'Rahal Amrith'

select * from payments

select empId, count(workedHours) as 'Total Worked Days',sum(workedHours) as 'Total Hours', sum(netSal) as 'Total Salary'
from payments
where workedDate >= '2019-08-01' and workedDate <= '2019-11-01'
group by empId


select p.empId, e.empName, e.hRate, count(p.workedHours) as 'Total Worked Days',sum(p.workedHours) as 'Total Hours', sum(p.netSal) as 'Total Salary'
from payments p, employee e
where p.workedDate >= '2019-08-01' and p.workedDate <= '2019-11-01' and e.empId = p.empId
group by p.empId, e.empName, e.hRate


select sum(netSal) as 'Total'
from payments
where workedDate >= '2019-08-01' and workedDate <= '2019-11-01'


select * from payments

select e.empId as 'ID', e.empName as 'Name', p.workedDate as 'Worked Date', p.bSal as 'Basic Salary', p.advance, p.deduction, p.netSal as 'Net Salary' from employee e, payments p where e.empId = p.empId and workedDate >= '2019-08-01' and workedDate <= '2019-09-01'
