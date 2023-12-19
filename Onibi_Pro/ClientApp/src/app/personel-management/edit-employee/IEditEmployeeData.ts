import { ManagerDetailsDto } from '../../api/api';
import { EmployeeRecord } from '../personel-management.component';

export interface IEditEmployeeData {
  employeeData: EmployeeRecord;
  managerDetails: ManagerDetailsDto;
}
