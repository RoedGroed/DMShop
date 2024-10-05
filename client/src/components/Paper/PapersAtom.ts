// src/atoms/PapersAtom.ts
import { atom } from "jotai";
import { ProductDto } from "../../Api";

export const PapersAtom = atom<ProductDto[]>([]);
