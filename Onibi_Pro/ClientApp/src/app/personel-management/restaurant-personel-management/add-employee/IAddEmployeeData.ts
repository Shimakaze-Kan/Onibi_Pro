import { GetManagerDetailsResponse } from '../../../api/api';

export interface IAddEmployeeData {
  managerDetails: GetManagerDetailsResponse;
  positions: Array<string>;
}
