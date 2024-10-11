// src/atoms/PapersAtom.ts
import { atom } from "jotai";
import { ProductDto } from "../Api.ts";

export const PapersAtom = atom<ProductDto[]>([]);
