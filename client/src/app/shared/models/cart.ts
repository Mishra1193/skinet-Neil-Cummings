// Types describing the shape
export type CartItem = {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: string;
  type: string;
};

export type CartType = {
  id: string;
  items: CartItem[];
};

// Class to allow initialization (implements the type)
import { nanoid } from 'nanoid';

export class Cart implements CartType {
  id: string = nanoid();       // client-side random ID
  items: CartItem[] = [];
}
