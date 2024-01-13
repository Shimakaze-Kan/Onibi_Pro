import { GetRegionalManagerResponse_RegionalManagerItem } from '../../../api/api';

export interface IEditRegionalManagerData {
  restaurants: Array<string>;
  regionalManager: GetRegionalManagerResponse_RegionalManagerItem;
}
