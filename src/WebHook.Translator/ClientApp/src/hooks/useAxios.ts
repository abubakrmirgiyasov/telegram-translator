import { useEffect, useState } from "react";
import axios, { AxiosInstance } from "axios";
import { Constants, RequestType } from "../common/Constants";
import { useProfile } from "./useProfile";

export interface IUseAxiosOptions {
  url: string;
  request: RequestType;
  headers?: Record<string, string>;
}

interface IUseAxiosResponse<T> {
  data: T | null;
  loading: boolean;
  error: Error | null;
}

export const useAxios = <T>(
  options: IUseAxiosOptions,
  body?: T
): IUseAxiosResponse<T> => {
  const user = useProfile();

  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<Error | null>(null);
  const [data, setData] = useState<T, null>(null);

  const axiosInstance: AxiosInstance = axios.create({
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${user.token}`,
      ...options.headers,
    },
  });

  switch (options.request) {
    case RequestType.get:
      axiosInstance
        .get<T>(options.url)
        .then((r) => setData(r))
        .catch((e) => setError(e))
        .finally(() => setLoading(false));
      break;
    case RequestType.post:
      axiosInstance
        .post<T>(options.url, body)
        .then((r) => setData(r))
        .catch((e) => setError(e))
        .finally(() => setLoading(false));
      break;
    case RequestType.put:
      axiosInstance
        .put<T>(options.url, body)
        .then((r) => setData(r))
        .catch((e) => setError(e))
        .finally(() => setLoading(false));
      break;
    case RequestType.delete:
      axiosInstance
        .delete(options.url)
        .then((r) => setData(r))
        .catch((e) => setError(e))
        .finally(() => setLoading(false));
      break;
    default:
      setLoading(false);
      break;
  }

  return { data, error, loading };
};
