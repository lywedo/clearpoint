import { useState, useCallback } from 'react';
import axios, { AxiosRequestConfig } from 'axios';

interface FetchState<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
  fetchData: () => void;
}

const useFetchTodoItems = <T>(endpoint: string, config?: AxiosRequestConfig): FetchState<T> => {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const url = `${process.env.REACT_APP_API_URL}/${endpoint}`;

  const fetchData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<T>(url, config);
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

  return { data, loading, error, fetchData };
};

export default useFetchTodoItems;
