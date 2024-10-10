import { atom } from "jotai";
import { PropertyDto } from "../Api.ts";

export const PropertiesAtom = atom<PropertyDto[]>([]);
