import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import {AddTodoItem} from '../AddTodoItem';

jest.mock('../../hooks/usePostTodoItem', () => ({
  __esModule: true,
  default: () => ({
    data: null,
    loading: false,
    error: null,
    postData: jest.fn(),
    cleanData: jest.fn(),
  }),
}));

describe('AddTodoItem component', () => {
  test('renders the component', () => {
    render(<AddTodoItem />);
    expect(screen.getByText('Add Item')).toBeInTheDocument();
  });

  test('handles description change', () => {
    render(<AddTodoItem />);
    const descriptionInput = screen.getByPlaceholderText('Enter description...');
    fireEvent.change(descriptionInput, { target: { value: 'Test description' } });
    expect(descriptionInput.nodeValue).toBe('Test description');
  });

  test('calls postData when Add Item button is clicked', () => {
    render(<AddTodoItem />);
    const addBtn = screen.getByText('Add Item');
    fireEvent.click(addBtn);
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    // Here, you can test further based on the expected behavior after posting data.
  });

  test('calls cleanData when Clear button is clicked', () => {
    render(<AddTodoItem />);
    const clearBtn = screen.getByText('Clear');
    fireEvent.click(clearBtn);
    expect(screen.getByPlaceholderText('Enter description...').nodeName).toBe('');
  });
});
