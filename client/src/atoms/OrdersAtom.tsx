import { atom } from "jotai";
import { OrderListDto } from "../Api";

export const OrdersAtom = atom<OrderListDto[]>([]);
