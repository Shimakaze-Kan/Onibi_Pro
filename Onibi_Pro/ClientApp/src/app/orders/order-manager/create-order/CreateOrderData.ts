import { GetMenusResponse } from '../../../api/api';

export class CreateOrderData {
  private readonly _restaurantId: string;
  private readonly _menus: Array<GetMenusResponse>;

  get restaurantId(): string {
    return this._restaurantId;
  }

  get menus(): Array<GetMenusResponse> {
    return this._menus;
  }

  constructor(restaurantId: string, menus: Array<GetMenusResponse>) {
    this._restaurantId = restaurantId;
    this._menus = menus;
  }
}
