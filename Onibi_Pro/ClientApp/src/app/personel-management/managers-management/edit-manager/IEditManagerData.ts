import { GetManagersResponse } from '../../../api/api';

export interface IEditManagerData {
  restaurants: Array<string>;
  manager: GetManagersResponse;
}
