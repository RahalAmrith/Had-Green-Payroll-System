use hadGreenPayroll

select workedDate as 'Worked Date', inTime as 'In Time', outTime as 'Out Time', workedHours as 'Worked Hours', bSal as 'Basic Salary', advance as 'Advance', deduction as 'Deduction', netSal as 'Net Salary'
from payments 
where empId = (select empId from employee where empName = 'Rahal Amrith') and 
	  workedDate >= '2019-01-01' and 
	  workedDate <= '2019-01-20'