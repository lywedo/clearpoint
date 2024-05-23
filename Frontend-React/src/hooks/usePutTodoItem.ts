import { useState, useCallback } from 'react';
import axios, { AxiosRequestConfig } from 'axios';

interface PutState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  putData: (body: any, id: string) => void;
}

const usePutTodoItem = <T>(endpoint: string, config?: AxiosRequestConfig): PutState<T> => {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const url = `${process.env.REACT_APP_API_URL}/${endpoint}`;

  const putData = useCallback(async (body: any, id: string) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.put<T>(`${url}/${id}`, body, config);
      setData(response.data);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(err.response?.data);
      } else {
        setError('An unexpected error occurred');
      }
    } finally {
      setLoading(false);
    }
  }, [url, config]);

  return { data, loading, error, putData };
};

export default usePutTodoItem;
