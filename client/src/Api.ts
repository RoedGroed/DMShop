/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface OrderListDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  customerId?: number | null;
  customerName?: string;
  /** @format date-time */
  orderDate?: string;
  /** @format date */
  deliveryDate?: string | null;
  /** @format double */
  totalAmount?: number;
  status?: string;
}

export interface OrderDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  customerId?: number | null;
  /** @format date-time */
  orderDate?: string | null;
  /** @format date-time */
  deliveryDate?: string | null;
  status?: string;
  /** @format double */
  totalAmount?: number;
  orderEntries?: OrderEntryDto[];
}

export interface OrderEntryDto {
  /** @format int32 */
  id?: number;
  /** @format int32 */
  quantity?: number;
  /** @format int32 */
  productId?: number | null;
  productName?: string | null;
  /** @format double */
  price?: number | null;
}

export interface CreateOrderDTO {
  /** @format int32 */
  customerId?: number;
  items?: OrderEntryRequestDTO[];
  /** @format date-time */
  deliveryDate?: string;
  /** @format date-time */
  orderDate?: string;
  status?: string;
  orderEntries?: OrderEntryRequestDTO[];
  /** @format double */
  totalAmount?: number;
}

export interface OrderEntryRequestDTO {
  /** @format int32 */
  productId?: number;
  /** @format int32 */
  quantity?: number;
}

export interface ProductDto {
  /** @format int32 */
  id?: number;
  name?: string;
  discontinued?: boolean;
  /** @format int32 */
  stock?: number;
  /** @format double */
  price?: number;
  properties?: PropertyDto[];
}

export interface PropertyDto {
  /** @format int32 */
  id?: number;
  propertyName?: string;
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "http://localhost:5000" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title My Title
 * @version 1.0.0
 * @baseUrl http://localhost:5000
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Order
     * @name OrderGetOrdersForList
     * @request GET:/api/Order
     */
    orderGetOrdersForList: (
      query?: {
        /**
         * @format int32
         * @default 10
         */
        limit?: number;
        /**
         * @format int32
         * @default 0
         */
        startAt?: number;
      },
      params: RequestParams = {},
    ) =>
      this.request<OrderListDto[], any>({
        path: `/api/Order`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Order
     * @name OrderCreateOrder
     * @request POST:/api/Order/create
     */
    orderCreateOrder: (data: CreateOrderDTO, params: RequestParams = {}) =>
      this.request<OrderDto, any>({
        path: `/api/Order/create`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductGetAllPapers
     * @request GET:/api/Product/basic
     */
    productGetAllPapers: (params: RequestParams = {}) =>
      this.request<ProductDto[], any>({
        path: `/api/Product/basic`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductGetAllPapersWithProperties
     * @request GET:/api/Product/with-properties
     */
    productGetAllPapersWithProperties: (params: RequestParams = {}) =>
      this.request<ProductDto[], any>({
        path: `/api/Product/with-properties`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductCreatePaper
     * @request POST:/api/Product
     */
    productCreatePaper: (data: ProductDto, params: RequestParams = {}) =>
      this.request<ProductDto, any>({
        path: `/api/Product`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductDeletePaper
     * @request DELETE:/api/Product/{id}
     */
    productDeletePaper: (id: number, data: ProductDto, params: RequestParams = {}) =>
      this.request<ProductDto, any>({
        path: `/api/Product/${id}`,
        method: "DELETE",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductUpdatePaper
     * @request PUT:/api/Product/{id}
     */
    productUpdatePaper: (id: number, data: ProductDto, params: RequestParams = {}) =>
      this.request<ProductDto, any>({
        path: `/api/Product/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Product
     * @name ProductGetPaperById
     * @request GET:/api/Product/{id}
     */
    productGetPaperById: (id: number, params: RequestParams = {}) =>
      this.request<ProductDto, any>({
        path: `/api/Product/${id}`,
        method: "GET",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Property
     * @name PropertyGetAllProperties
     * @request GET:/api/Property
     */
    propertyGetAllProperties: (params: RequestParams = {}) =>
      this.request<PropertyDto[], any>({
        path: `/api/Property`,
        method: "GET",
        format: "json",
        ...params,
      }),
  };
}
