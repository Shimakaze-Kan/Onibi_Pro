import { GetManagerDetailsResponse } from '../../../api/api';
import { EmployeeRecord } from '../restaurant-personel-management.component';

export interface IEditEmployeeData {
  employeeData: EmployeeRecord;
  managerDetails: GetManagerDetailsResponse;
  positions: Array<string>;
}
